module day09_part02

open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day09_input.txt"

let execute =
    IntcodeComputerModule.executeBigData(filepath, 2I)