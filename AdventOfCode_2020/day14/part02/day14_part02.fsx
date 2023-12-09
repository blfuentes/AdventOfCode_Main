open System.Collections.Generic

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"

open AoC_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day14_input.txt"
let path = "day14/" + file
let inputLines = GetLinesFromFile(path)