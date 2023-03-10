module day01_part01

open Utilities

let path = "day01/day01_input.txt"

let execute = 
    let inputline: char[] = GetContentFromFile(path) |> Seq.toArray
    let elements2
        = ((inputline |> Array.filter (fun (x: char) -> x = '(')).Length , (inputline |> Array.filter (fun (x: char) -> x = ')')).Length)
    fst elements2 - snd elements2

