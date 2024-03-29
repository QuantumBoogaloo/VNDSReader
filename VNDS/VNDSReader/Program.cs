﻿using System;
using System.Collections.Generic;
using System.IO;
using VNDS;
using VNDS.Commands;
using VNDS.Commands.Visitors;

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

namespace VNDSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: VNDSReader <path> ...");
                return;
            }

            foreach (var arg in args)
            {
                try
                {
                    ProcessFile(arg);
                }
                catch (Exception ex)
                {
                    File.WriteAllText(Path.ChangeExtension(arg, ".errlog"), ex.Message);
                    Console.Error.WriteLine(ex.Message);
                }
            }
        }

        static void ProcessFile(string path)
        {
            using (var reader = new TextCharReader(new StreamReader(path)))
            {
                var parser = new Parser(reader);

                var commands = new List<Command>(FilterErroneousCommands(parser.ParseCommands()));

                using (var writer = new StreamWriter(Path.ChangeExtension(path, ".vnvita")))
                {
                    var formatter = new CommandFormatVisitor(writer);

                    foreach (var command in commands)
                        formatter.Visit(command);
                }
            }
        }

        static IEnumerable<Command> FilterErroneousCommands(IEnumerable<Command> commands)
        {
            var enumerator = commands.GetEnumerator();

            bool inErroneousIfBlock = false;

            while (enumerator.MoveNext())
            {
                if (inErroneousIfBlock)
                {
                    if (enumerator.Current is FiCommand)
                        inErroneousIfBlock = false;
                }
                else if (enumerator.Current.IsErroneous)
                {
                    if (enumerator.Current is IfCommand)
                        inErroneousIfBlock = true;
                }
                else
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}
