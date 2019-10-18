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

namespace VNDS.Commands
{
    public abstract class CommandVisitor
    {
        public void Visit(Command command)
        {
            command.Accept(this);
        }

        protected internal abstract void VisitSkipCommand(SkipCommand skipCommand);
        protected internal abstract void VisitEndScriptCommand(EndScriptCommand endScriptCommand);

        protected internal abstract void VisitBackgroundLoadCommand(BackgroundLoadCommand backgroundLoadCommand);

        protected internal abstract void VisitSetImageCommand(SetImageCommand setImageCommand);

        protected internal abstract void VisitChoiceCommand(ChoiceCommand choiceCommand);

        protected internal abstract void VisitJumpCommand(JumpCommand jumpCommand);

        protected internal abstract void VisitDelayCommand(DelayCommand delayCommand);

        protected internal abstract void VisitRandomCommand(RandomCommand randomCommand);

        protected internal abstract void VisitLabelCommand(LabelCommand labelCommand);

        protected internal abstract void VisitGoToCommand(GoToCommand gotoCommand);

        protected internal abstract void VisitClearTextCommand(ClearTextCommand clearTextCommand);

        protected internal abstract void VisitIfCommand(IfCommand ifCommand);
        protected internal abstract void VisitFiCommand(FiCommand fiCommand);

        protected internal abstract void VisitAwaitInputCommand(AwaitInputCommand awaitInputCommand);

        protected internal abstract void VisitTextCommand(TextCommand textCommand);

        protected internal abstract void VisitSetLocalVariableCommand(SetLocalVariableCommand setLocalVariableCommand);
        protected internal abstract void VisitClearLocalVariablesCommand(ClearLocalVariablesCommand clearLocalVariablesCommand);

        protected internal abstract void VisitSetGlobalVariableCommand(SetGlobalVariableCommand setGlobalVariableCommand);
        protected internal abstract void VisitClearGlobalVariablesCommand(ClearGlobalVariablesCommand clearGlobalVariablesCommand);

        protected internal abstract void VisitPlayMusicCommand(PlayMusicCommand playMusicCommand);
        protected internal abstract void VisitStopMusicCommand(StopMusicCommand stopMusicCommand);

        protected internal abstract void VisitPlaySoundCommand(PlaySoundCommand playSoundCommand);
        protected internal abstract void VisitStopSoundCommand(StopSoundCommand stopSoundCommand);
    }
}
