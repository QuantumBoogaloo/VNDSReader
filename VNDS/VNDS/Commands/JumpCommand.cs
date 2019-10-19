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
    public class JumpCommand : Command
    {
        private const string defaultLabel = "";

        private string path;
        private string label;

        public JumpCommand(string path, string label = defaultLabel)
            : base()
        {
            this.path = path;
            this.label = label;
        }

        public JumpCommand(string path, params ParseException[] exceptions)
            : this(path, defaultLabel, exceptions)
        {
        }

        public JumpCommand(string path, string label, params ParseException[] exceptions)
            : base(exceptions)
        {
            this.path = path;
            this.label = label;
        }

        public string Path
        {
            get { return this.path; }
        }

        public string Label
        {
            get { return this.label; }
        }

        public override void Accept(CommandVisitor visitor)
        {
            visitor.VisitJumpCommand(this);
        }
    }
}
