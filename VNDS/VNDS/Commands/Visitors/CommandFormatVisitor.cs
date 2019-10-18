using System;
using System.IO;

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

namespace VNDS.Commands.Visitors
{
    public class CommandFormatVisitor : CommandVisitor, IDisposable
    {
        private const string defaultIndent = "\t";

        private readonly TextWriter writer;
        private readonly bool shouldDispose = true;
        private int indentLevel = 0;
        private string indentString = defaultIndent;

        public CommandFormatVisitor(TextWriter writer, string indentString = defaultIndent)
        {
            this.writer = writer;
            this.indentString = indentString;
        }

        public CommandFormatVisitor(TextWriter writer, bool shouldDispose, string indentString = defaultIndent)
        {
            this.writer = writer;
            this.shouldDispose = shouldDispose;
            this.indentString = indentString;
        }

        public void Dispose()
        {
            if (this.shouldDispose)
                this.writer.Dispose();
        }

        private void IncreaseIndent()
        {
            ++this.indentLevel;
        }

        private void DecreaseIndent()
        {
            --this.indentLevel;
        }

        private void WriteIndent()
        {
            for (int i = 0; i < indentLevel; ++i)
                this.writer.Write(this.indentString);
        }

        protected string ToString(SetOperation operation)
        {
            switch (operation)
            {
                case SetOperation.Assign:
                    return "=";
                case SetOperation.Add:
                    return "+";
                case SetOperation.Subtract:
                    return "-";
                default:
                    return "";
            }
        }

        protected string ToString(IfOperation operation)
        {
            switch (operation)
            {
                case IfOperation.Equals:
                    return "==";
                case IfOperation.NotEquals:
                    return "!=";
                case IfOperation.GreaterThan:
                    return ">";
                case IfOperation.LessThan:
                    return "<";
                case IfOperation.GreaterThanEquals:
                    return ">=";
                case IfOperation.LessThanEquals:
                    return "<=";
                default:
                    return string.Empty;
            }
        }

        protected internal override void VisitSkipCommand(SkipCommand skipCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("#");
        }

        protected internal override void VisitEndScriptCommand(EndScriptCommand endScriptCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("endscript");
        }

        protected internal override void VisitBackgroundLoadCommand(BackgroundLoadCommand backgroundLoadCommand)
        {
            this.WriteIndent();
            if (backgroundLoadCommand.FadeTime < 0)
                this.writer.WriteLine("bgload {0}", backgroundLoadCommand.Path);
            else
                this.writer.WriteLine("bgload {0} {1}", backgroundLoadCommand.Path, backgroundLoadCommand.FadeTime);
        }

        protected internal override void VisitSetImageCommand(SetImageCommand setImageCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("setimg {0} {1} {2}", setImageCommand.Path, setImageCommand.X, setImageCommand.Y);
        }

        protected internal override void VisitChoiceCommand(ChoiceCommand choiceCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("choice {0}", string.Join(" | ", choiceCommand.Choices));
        }

        protected internal override void VisitJumpCommand(JumpCommand jumpCommand)
        {
            this.WriteIndent();
            if (string.IsNullOrWhiteSpace(jumpCommand.Label))
                this.writer.WriteLine("jump {0}", jumpCommand.Path);
            else
                this.writer.WriteLine("jump {0} {1}", jumpCommand.Path, jumpCommand.Label);
        }

        protected internal override void VisitDelayCommand(DelayCommand delayCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("delay {0}", delayCommand.Time);
        }

        protected internal override void VisitRandomCommand(RandomCommand randomCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("random {0} {1} {2}", randomCommand.Variable, randomCommand.Low, randomCommand.High);
        }

        protected internal override void VisitLabelCommand(LabelCommand labelCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("label {0}", labelCommand.Label);
        }

        protected internal override void VisitGoToCommand(GoToCommand gotoCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("goto {0}", gotoCommand.Label);
        }

        protected internal override void VisitClearTextCommand(ClearTextCommand clearTextCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("cleartext");
        }

        protected internal override void VisitIfCommand(IfCommand ifCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("if {0} {1} {2}", ifCommand.Left, this.ToString(ifCommand.Operation), ifCommand.Right);
            this.IncreaseIndent();
        }

        protected internal override void VisitFiCommand(FiCommand fiCommand)
        {
            this.DecreaseIndent();
            this.WriteIndent();
            this.writer.WriteLine("fi");
        }

        // TODO text ~, append blank line and don't wait for input

        protected internal override void VisitAwaitInputCommand(AwaitInputCommand awaitInputCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("text !");
        }

        protected internal override void VisitTextCommand(TextCommand textCommand)
        {
            this.WriteIndent();
            if(textCommand.Options == TextOptions.None)
                this.writer.WriteLine("text @{0}", textCommand.Text);
            else
                this.writer.WriteLine("text {0}", textCommand.Text);
        }

        protected internal override void VisitSetLocalVariableCommand(SetLocalVariableCommand setLocalVariableCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("setvar {0} {1} {2}", setLocalVariableCommand.Left, this.ToString(setLocalVariableCommand.Operation), setLocalVariableCommand.Right);
        }

        protected internal override void VisitClearLocalVariablesCommand(ClearLocalVariablesCommand clearLocalVariablesCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("setvar ~ ~");
        }

        protected internal override void VisitSetGlobalVariableCommand(SetGlobalVariableCommand setGlobalVariableCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("gsetvar {0} {1} {2}", setGlobalVariableCommand.Left, this.ToString(setGlobalVariableCommand.Operation), setGlobalVariableCommand.Right);
        }

        protected internal override void VisitClearGlobalVariablesCommand(ClearGlobalVariablesCommand clearGlobalVariablesCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("gsetvar ~ ~");
        }

        protected internal override void VisitPlayMusicCommand(PlayMusicCommand playMusicCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("music {0}", playMusicCommand.Path);
        }

        protected internal override void VisitStopMusicCommand(StopMusicCommand stopMusicCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("music ~");
        }

        protected internal override void VisitPlaySoundCommand(PlaySoundCommand playSoundCommand)
        {
            this.WriteIndent();
            if (playSoundCommand.Repeats > 1)
                this.writer.WriteLine("sound {0} {1}", playSoundCommand.Path, playSoundCommand.Repeats);
            else
                this.writer.WriteLine("sound {0}", playSoundCommand.Path);
        }

        protected internal override void VisitStopSoundCommand(StopSoundCommand stopSoundCommand)
        {
            this.WriteIndent();
            this.writer.WriteLine("sound ~");
        }
    }
}
