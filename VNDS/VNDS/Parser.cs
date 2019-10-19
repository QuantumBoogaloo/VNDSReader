using System;
using System.Collections.Generic;
using VNDS.Commands;

//
//  Copyright (C) 2019 Pharap (@Pharap)
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

namespace VNDS
{
    public class Parser
    {
        private static readonly ISet<char> delimiters = new HashSet<char>(" \t\r");
        private ICharReader reader;

        public Parser(ICharReader reader)
        {
            this.reader = reader;
        }

        public IEnumerable<Command> ParseCommands()
        {
            for (var command = this.ParseNextCommand(); command != null; command = this.ParseNextCommand())
                yield return command;
        }
        
        public Command ParseNextCommand()
        {
            string text;

            if (this.reader.TryReadUntilAny(out text, '\n'))
                return this.ParseNextCommand(new StringCharReader(text));

            return null;
        }

        private Command ParseNextCommand(ICharReader reader)
        {
            string name = reader.ReadUntilAny(delimiters);

            if ((name == null) || (name[0] == '#'))
                return new SkipCommand();

            switch (name)
            {
                case "bgload":
                    return this.ParseBackgroundLoadCommand(reader);
                case "setimg":
                    return this.ParseSetImageCommand(reader);
                case "sound":
                    return this.ParseSoundCommand(reader);
                case "music":
                    return this.ParseMusicCommand(reader);
                case "text":
                    return this.ParseTextCommand(reader);
                case "choice":
                    return this.ParseChoiceCommand(reader);
                case "setvar":
                    return this.ParseSetVariableCommand(reader);
                case "gsetvar":
                    return this.ParseGlobalSetVariableCommand(reader);
                case "if":
                    return this.ParseIfCommand(reader);
                case "fi":
                    return new FiCommand();
                case "jump":
                    return this.ParseJumpCommand(reader);
                case "delay":
                    return this.ParseDelayCommand(reader);
                case "random":
                    return this.ParseRandomCommand(reader);
                case "label":
                    return this.ParseLabelCommand(reader);
                case "goto":
                    return this.ParseGoToCommand(reader);
                case "cleartext":
                    return new ClearTextCommand();
                case "endscript":
                    return new EndScriptCommand();
                default:
                    return new SkipCommand();
            }
        }

        private BackgroundLoadCommand ParseBackgroundLoadCommand(ICharReader reader)
        {
            string path;
            if (!TryReadString(reader, out path))
                return new BackgroundLoadCommand(path, new ParseException("Unable to parse <path>"));

            int fadeTime;
            if (TryReadInt(reader, out fadeTime))
                return new BackgroundLoadCommand(path, fadeTime);
            else
                return new BackgroundLoadCommand(path);
        }

        private SetImageCommand ParseSetImageCommand(ICharReader reader)
        {
            string path;
            if (!TryReadString(reader, out path))
                return new SetImageCommand("", 0, 0, new ParseException("Unable to parse <path>"));

            int x;
            if (!TryReadInt(reader, out x))
                return new SetImageCommand(path, x, 0, new ParseException("Unable to parse <x>"));

            int y;
            if (!TryReadInt(reader, out y))
                return new SetImageCommand(path, x, y, new ParseException("Unable to parse <y>"));

            return new SetImageCommand(path, x, y);
        }

        private SoundCommand ParseSoundCommand(ICharReader reader)
        {
            string path;
            if (!TryReadString(reader, out path))
                return new PlaySoundCommand(path, new ParseException("Unable to parse <path>"));
            
            if (path.Trim() == "~")
                return new StopSoundCommand();

            int repeats;
            if (TryReadInt(reader, out repeats))
                return new PlaySoundCommand(path, repeats);
            else
                return new PlaySoundCommand(path);
        }

        private MusicCommand ParseMusicCommand(ICharReader reader)
        {
            string path;
            if (!TryReadString(reader, out path))
                return new PlayMusicCommand(path, new ParseException("Unable to parse <path>"));

            if (path.Trim() == "~")
                return new StopMusicCommand();
            else
                return new PlayMusicCommand(path);
        }

        private Command ParseTextCommand(ICharReader reader)
        {
            string text = reader.ReadUntilAny('\n');

            if (text == null)
                return new TextCommand("", TextOptions.AwaitInput);

            switch(text[0])
            {
                case '~':
                    return new TextCommand("", TextOptions.None);

                case '!':
                    return new AwaitInputCommand();

                case '@':
                    return new TextCommand(text.Substring(1), TextOptions.None);
            }

            return new TextCommand(text, TextOptions.AwaitInput);
        }

        private ChoiceCommand ParseChoiceCommand(ICharReader reader)
        {
            var choices = new List<string>();

            for (var choice = reader.ReadUntilAny('|'); choice != null; choice = reader.ReadUntilAny('|'))
                choices.Add(choice);

            return new ChoiceCommand(choices);
        }

        private Command ParseSetVariableCommand(ICharReader reader)
        {
            string left;
            if (!TryReadString(reader, out left))
                return new SetLocalVariableCommand("", SetOperation.Add, "", new ParseException("Unable to parse <left>"));

            string op;
            if (!TryReadString(reader, out op))
                return new SetLocalVariableCommand(left, SetOperation.Add, "", new ParseException("Unable to parse <operation>"));

            if (op.Trim() == "~")
                return new ClearLocalVariablesCommand();

            SetOperation operation;
            if (!TryReadSetOperation(op, out operation))
                return new SetLocalVariableCommand(left, SetOperation.Add, "", new ParseException("Unable to parse <operation>"));

            string right;
            if (TryReadString(reader, out right))
                return new SetLocalVariableCommand(left, operation, right);
            else
                return new SetLocalVariableCommand(left, operation, "", new ParseException("Unable to parse <right>"));
        }

        private Command ParseGlobalSetVariableCommand(ICharReader reader)
        {
            string left;
            if (!TryReadString(reader, out left))
                return new SetGlobalVariableCommand("", SetOperation.Add, "", new ParseException("Unable to parse <left>"));

            string op;
            if (!TryReadString(reader, out op))
                return new SetGlobalVariableCommand(left, SetOperation.Add, "", new ParseException("Unable to parse <operation>"));

            if (op.Trim() == "~")
                return new ClearGlobalVariablesCommand();

            SetOperation operation;
            if (!TryReadSetOperation(op, out operation))
                return new SetGlobalVariableCommand(left, SetOperation.Add, "", new ParseException("Unable to parse <operation>"));

            string right;
            if (TryReadString(reader, out right))
                return new SetGlobalVariableCommand(left, operation, right);
            else
                return new SetGlobalVariableCommand(left, operation, "", new ParseException("Unable to parse <right>"));
        }

        private IfCommand ParseIfCommand(ICharReader reader)
        {
            string left;
            if (!TryReadString(reader, out left))
                return new IfCommand("", IfOperation.Equals, "", new ParseException("Unable to parse <left>"));

            string op;
            if (!TryReadString(reader, out op))
                return new IfCommand(left, IfOperation.Equals, "", new ParseException("Unable to parse <operation>"));

            IfOperation operation;
            if (!TryReadIfOperation(op, out operation))
                return new IfCommand(left, IfOperation.Equals, "", new ParseException("Unable to parse <operation>"));

            string right;
            if (TryReadString(reader, out right))
                return new IfCommand(left, operation, right);
            else
                return new IfCommand(left, operation, "", new ParseException("Unable to parse <right>"));
        }

        private JumpCommand ParseJumpCommand(ICharReader reader)
        {
            string path;
            if (!TryReadString(reader, out path))
                return new JumpCommand(path, new ParseException("Unable to parse <path>"));

            string label;
            if (TryReadString(reader, out label))
                return new JumpCommand(path, label);
            else
                return new JumpCommand(path);
        }

        private DelayCommand ParseDelayCommand(ICharReader reader)
        {
            int delay;
            if (!TryReadInt(reader, out delay))
                return new DelayCommand(0, new ParseException("Unable to parse <delay>"));

            return new DelayCommand(delay);
        }

        private RandomCommand ParseRandomCommand(ICharReader reader)
        {
            string variable;
            if (!TryReadString(reader, out variable))
                return new RandomCommand("", 0, 0, new ParseException("Unable to parse <variable>"));

            int low;
            if (!TryReadInt(reader, out low))
                return new RandomCommand(variable, low, 0, new ParseException("Unable to parse <low>"));

            int high;
            if (!TryReadInt(reader, out high))
                return new RandomCommand(variable, low, high, new ParseException("Unable to parse <low>"));

            return new RandomCommand(variable, low, high);
        }

        private LabelCommand ParseLabelCommand(ICharReader reader)
        {
            string label;
            if (!TryReadString(reader, out label))
                return new LabelCommand("", new ParseException("Unable to parse <label>"));

            return new LabelCommand(label);
        }

        private GoToCommand ParseGoToCommand(ICharReader reader)
        {
            string label;
            if (!TryReadString(reader, out label))
                return new GoToCommand("", new ParseException("Unable to parse <label>"));

            return new GoToCommand(label);
        }

        private static bool TryReadString(ICharReader reader, out string result)
        {
            return reader.TryReadUntilAny(out result, delimiters);
        }

        private static bool TryReadInt(ICharReader reader, out int result)
        {
            string text;
            if (!TryReadString(reader, out text))
            {
                result = 0;
                return false;
            }

            return (int.TryParse(text, out result));
        }

        private static bool TryReadSetOperation(string op, out SetOperation operation)
        {
            switch (op.Trim())
            {
                case "=":
                    operation = SetOperation.Assign;
                    return true;
                case "+":
                    operation = SetOperation.Add;
                    return true;
                case "-":
                    operation = SetOperation.Subtract;
                    return true;
                default:
                    operation = default(SetOperation);
                    return false;
            }
        }

        private static bool TryReadIfOperation(string op, out IfOperation operation)
        {
            switch (op.Trim())
            {
                case "==":
                    operation = IfOperation.Equals;
                    return true;
                case "!=":
                    operation = IfOperation.NotEquals;
                    return true;
                case "<":
                    operation = IfOperation.LessThan;
                    return true;
                case ">":
                    operation = IfOperation.GreaterThan;
                    return true;
                case "<=":
                    operation = IfOperation.LessThanEquals;
                    return true;
                case ">=":
                    operation = IfOperation.GreaterThanEquals;
                    return true;
                default:
                    operation = default(IfOperation);
                    return false;
            }
        }
    }
}
