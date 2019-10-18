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
    public class SetImageCommand : Command
    {
        private string path;
        private int x;
        private int y;

        public SetImageCommand(string path, int x, int y)
        {
            this.path = path;
            this.x = x;
            this.y = y;
        }

        public string Path
        {
            get { return this.path; }
        }

        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }

        public override void Accept(CommandVisitor visitor)
        {
            visitor.VisitSetImageCommand(this);
        }
    }
}
