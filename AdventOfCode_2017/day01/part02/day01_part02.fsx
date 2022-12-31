#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_2017.Modules

//let path = "day01/test_input_01.txt"
let path = "day01/day01_input.txt"

let value0 = "1212"
let value1 = "1221"
let value2 = "123425"
let value3 = "123123"
let value4 = "12131415"

let calculateCaptcha (captcha: string) (offset: int)=
    let values = 
        seq {
            for idx in [0..captcha.Length - 1] do
                let val1 = captcha.Substring(idx, 1)
                let idx2 = if idx + offset <= captcha.Length - 1 then idx + offset else offset - (captcha.Length - idx)
                let val2 = captcha.Substring(idx2, 1)
                // printfn "Offset: %i Comparing val 1: %s at idx %i with val 2: %s at idx %i" offset val1 idx val2 idx2
                if val1 = val2 then
                    yield int(val1)
        }
    values |> Seq.sum

calculateCaptcha value0 (value0.Length / 2)
calculateCaptcha value1 (value1.Length / 2)
calculateCaptcha value2 (value2.Length / 2)
calculateCaptcha value3 (value3.Length / 2)
calculateCaptcha value4 (value4.Length / 2)

let value = Utilities.GetContentFromFile(path)
calculateCaptcha value (value.Length / 2)