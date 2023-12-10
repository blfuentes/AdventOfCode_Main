namespace AdventOfCode_2023.Modules

open System
open System.IO

module LocalHelper =
    let ReadLines(path: string) =
        File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path)

