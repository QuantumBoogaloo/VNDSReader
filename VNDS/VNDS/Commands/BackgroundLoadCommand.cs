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
    public class BackgroundLoadCommand : Command
    {
        private string path;
        private int fadeTime;

        public BackgroundLoadCommand(string path, int fadeTime = -1)
        {
            this.path = path;
            this.fadeTime = fadeTime;
        }

        public string Path
        {
            get { return this.path; }
        }

        public int FadeTime
        {
            get { return this.fadeTime; }
        }

        public override void Accept(CommandVisitor visitor)
        {
            visitor.VisitBackgroundLoadCommand(this);
        }
    }
}
