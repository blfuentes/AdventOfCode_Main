module day04_part01

open AdventOfCode_2024.Modules
open System

let parseContent (lines: string array) =
    let map = Array2D.create lines.Length lines[0].Length ""
    for rowIdx in [0..lines.Length - 1] do
        for colIdx in [0..lines[0].Length - 1] do         
            let row = lines[rowIdx].ToCharArray()
            map[rowIdx, colIdx] <- (string)row[colIdx]
    map

let getDiagonals (matrix: string[,]) =
    let rows = Array2D.length1 matrix
    let cols = Array2D.length2 matrix

    // diagonals from left to right
    let leftToRightDiagonals =
        [ for d in 0 .. rows + cols - 2 do
            [ for i in 0 .. d do
                let j = d - i
                if i < rows && j < cols then yield matrix[i, j] ] ]

    // diagonals from right to left
    let rightToLeftDiagonals =
        [ for d in 0 .. rows + cols - 2 do
            [ for i in 0 .. d do
                let j = cols - 1 - (d - i)
                if i < rows && j >= 0 then yield matrix[i, j] ] ]

    leftToRightDiagonals @ rightToLeftDiagonals

let getRow (matrix: string[,]) row = 
    let cols = Array2D.length2 matrix 
    [| for col in 0 .. cols - 1 do yield matrix[row, col] |]

let getCol (matrix: string[,]) col = 
    let rows = Array2D.length1 matrix 
    [| for row in 0 .. rows - 1 do yield matrix[row, col] |]

let countTimesOverlapped (pattern: string) (input: string) =
    let rec countFromIndex index count =
        match input.IndexOf(pattern, index, StringComparison.OrdinalIgnoreCase) with
        | -1 -> count
        | i -> countFromIndex (i + 1) (count + 1)

    countFromIndex 0 0

let countXmas(map: string [,]) =
    let maxRows = map.GetLength(0)
    let maxCols = map.GetLength(1)
    let mutable count = 0
    for rowIdx in [0..maxRows-1] do
        let row = String.concat "" (getRow  map rowIdx) 
        count <- count + countTimesOverlapped "XMAS" row
        count <- count + countTimesOverlapped "SAMX" row
    for colIdx in [0..maxCols-1] do
        let col = String.concat "" (getCol  map colIdx) 
        count <- count + countTimesOverlapped "XMAS" col
        count <- count + countTimesOverlapped "SAMX" col    
    let diagonals = getDiagonals map |> List.map(fun d -> String.concat "" d)
    let foundIndDiagonals = 
        diagonals
        |> List.map(fun d -> 
            (countTimesOverlapped "XMAS" d) +
            (countTimesOverlapped "SAMX" d))
        |> List.sum

    count <- count + foundIndDiagonals

    count

let execute =
    let path = "day04/day04_input.txt"
    //let path = "day04/test_input_01.txt"
    let content = LocalHelper.GetLinesFromFile path
    let map = parseContent content
    countXmas map