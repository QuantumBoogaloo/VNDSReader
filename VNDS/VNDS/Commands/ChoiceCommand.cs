﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    public class ChoiceCommand : Command
    {
        private string[] choices;
        private ReadOnlyCollection<string> choicesWrapper;

        public ChoiceCommand(IEnumerable<string> choices)
            : base()
        {
            this.choices = choices.ToArray();
            this.choicesWrapper = new ReadOnlyCollection<string>(this.choices);
        }

        public ChoiceCommand(IEnumerable<string> choices, params ParseException[] exceptions)
            : base(exceptions)
        {
            this.choices = choices.ToArray();
            this.choicesWrapper = new ReadOnlyCollection<string>(this.choices);
        }

        public ReadOnlyCollection<string> Choices
        {
            get { return this.choicesWrapper; }
        }

        public override void Accept(CommandVisitor visitor)
        {
            visitor.VisitChoiceCommand(this);
        }
    }
}
