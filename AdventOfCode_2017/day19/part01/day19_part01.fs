module day19_part01

open AdventOfCode_2017.Modules

type PathKind =
    | Vertical
    | Horizontal
    | Cross
    | Empty

type Cell = {
    Row: int
    Col: int
    Kind: PathKind
    Value: char option
}

let IsPathKind (cell: Cell) (map: Cell[,]) =
    let (up, down, left, right) = (map[cell.Row-1, cell.Col], map[cell.Row+1, cell.Col], map[cell.Row, cell.Col-1], map[cell.Row, cell.Col+1])
    match (up, down, left, right) with
    | (u, d, l, r) when u.Kind.IsVertical || u.Kind.IsVertical -> 

let parseContent(lines: string array) =
    let (maxrows, maxcols) = (lines.Length, lines.[0].Length)
    let grid = Array2D.init maxrows maxcols (fun r c -> { Row = r; Col = c; Kind = PathKind.Empty; Value = None })

    let checkpoints =
        [for row in 0..maxrows-1 do
            for col in 0..maxcols-1 do
                if lines[row][col] = '|' then
                    grid[row, col] <- { grid[row, col] with Kind = PathKind.Vertical }
                elif lines[row][col] = '-' then
                    grid[row, col] <- { grid[row, col] with Kind = PathKind.Horizontal }
                elif lines[row][col] = '+' then
                    grid[row, col] <- { grid[row, col] with Kind = PathKind.Cross }
                elif lines[row][col] <> ' ' then
                    grid[row, col] <- { grid[row, col] with Value = Some(lines[row][col]) }
                    yield grid[row, col]
        ]
    (grid, checkpoints)

let execute() =
    //let path = "day19/day19_input.txt"
    let path = "day19/test_input_19.txt"
    let content = LocalHelper.GetLinesFromFile path
    
    let (map, checkpoints) = parseContent content
    checkpoints.Length