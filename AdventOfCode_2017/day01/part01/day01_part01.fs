module day01_part01

open System
open System.Collections.Generic

open AdventOfCode_Utilities

let path = "day01/day01_input.txt"

let calculateCaptcha (captcha: string) =
    let values = 
        seq {
            for idx in [0..captcha.Length - 1] do
                if idx < captcha.Length - 1 && captcha.Substring(idx, 1) = captcha.Substring(idx + 1, 1) then
                    yield int(captcha.Substring(idx, 1))
            if captcha.Substring(captcha.Length - 1, 1) = captcha.Substring(0, 1) then
                yield int(captcha.Substring(0, 1))
        }
    values |> Seq.sum

let execute =
    calculateCaptcha (Utilities.GetContentFromFile(path))