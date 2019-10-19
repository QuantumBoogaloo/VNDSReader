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
    public class PlaySoundCommand : SoundCommand
    {
        private const int defaultRepeats = 1;

        private string path;
        private int repeats;

        public PlaySoundCommand(string path, int repeats = defaultRepeats)
            : base()
        {
            this.path = path;
            this.repeats = repeats;
        }

        public PlaySoundCommand(string path, int repeats, params ParseException[] exceptions)
            : base(exceptions)
        {
            this.path = path;
            this.repeats = repeats;
        }

        public PlaySoundCommand(string path, params ParseException[] exceptions)
            : this(path, defaultRepeats, exceptions)
        {
        }

        public string Path
        {
            get { return this.path; }
        }

        public int Repeats
        {
            get { return this.repeats; }
        }

        public override void Accept(CommandVisitor visitor)
        {
            visitor.VisitPlaySoundCommand(this);
        }
    }
}
