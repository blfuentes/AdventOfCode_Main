module day13_part01

open AdventOfCode_2016.Modules
open AdventOfCode_Utilities

let buildCell((row, col): int*int) (favouritenumber: int)=
    // x*x + 3*x + 2*x*y + y + y*y
    //let r1 = row*row + 3*row + 2*row*col + col + col*col
    let r1 = col*col + 3*col + 2*col*row + row + row*row

    let r2 = r1 + favouritenumber
    let r3 = System.Convert.ToString(r2, 2).ToCharArray() |> Array.filter(fun c-> c = '1') |> Array.length
    if r3 % 2 = 0 then " " else "#"

let buildMap((maxrows, maxcols): int*int, favouritenumber: int) =
    let mymap = Array2D.create maxrows maxcols ""
    for row in 0..(maxrows - 1) do
        for col in 0..(maxcols - 1) do
            mymap[row, col] <- buildCell (row, col) favouritenumber
    mymap
   
let printmap (mymap: string[,])  =
    let maxrows, maxcols = (mymap.GetLength(0), mymap.GetLength(1))
    for row in 0..(maxrows - 1) do
        for col in 0..(maxcols - 1) do
            printf "%s" mymap[row, col]
        printfn ""


let execute =
    let path = "day13/day13_input.txt"
    let favouritenumber = LocalHelper.GetContentFromFile path |> int
    let favouritenumber = 10
    let map = buildMap((7,10), favouritenumber)
    printmap map 
    0