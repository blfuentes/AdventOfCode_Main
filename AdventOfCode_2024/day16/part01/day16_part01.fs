module day16_part01

open AdventOfCode_2024.Modules
open System.Collections.Generic

type CellType = 
    | WALL
    | EMPTY
    | START
    | END

type Cell = {
    Kind: CellType
    Row: int;
    Col: int;
    Orientation : int
}

let parseContent(lines: string array) =
    let maxrows = lines.Length
    let maxcols = lines[0].Length
    let map = Array2D.init maxrows maxcols (fun row col -> { Kind = WALL; Row = row; Col = col; Orientation = -1 })
    let mutable Start = { Kind = START; Row = -1; Col = -1; Orientation = 2 }
    let mutable End = { Kind = END; Row = -1; Col = -1; Orientation = 0 }
    for row in 0..(maxrows-1) do
        let line = lines[row].ToCharArray()        
        for col in 0..(maxcols-1) do
            let value = line[col]
            if value <> '#' then
                map[row, col] <- { map[row, col] with Kind = EMPTY } 
                if value = 'S' then
                    Start <- { Start with Row = row; Col = col }
                elif value = 'E' then
                    map[row, col] <- { map[row, col] with Kind = EMPTY } 
                    End <- { End with Row = row; Col = col; }
    (map, Start, End)

let minValueAtCell (dimensions: int[][][]) (endnode: Cell) =
    dimensions[endnode.Row][endnode.Col]
    |> Array.min
 
let walk ((startnode, endnode):Cell*Cell) (dimensions: int array array array) (map: Cell[,]) =
    let queue = new Queue<Cell>()

    let rowDirs = [|-1; 0; 1; 0 |]
    let colDirs = [|0; -1; 0; 1 |]

    let add  (cell: Cell) (cost: int) =
        let currentCost = dimensions[cell.Row][cell.Col][cell.Orientation]
        if not cell.Kind.IsWALL && currentCost > cost then
            dimensions[cell.Row][cell.Col][cell.Orientation] <- cost
            queue.Enqueue(cell)

    add startnode 0
    while queue.Count > 0 do
        let newcell = queue.Dequeue()
        let distance = dimensions[newcell.Row][newcell.Col][newcell.Orientation]
        let (newRow, newCol) = (newcell.Row + rowDirs[newcell.Orientation], newcell.Col + colDirs[newcell.Orientation])
        let cell1 = { 
            Kind = map[newRow, newCol].Kind        
            Row = newRow;
            Col = newCol;
            Orientation = newcell.Orientation
        }
        add cell1 (distance+1)

        let cell2 = { 
            Kind = newcell.Kind        
            Row = newcell.Row;
            Col = newcell.Col;
            Orientation = (newcell.Orientation + 1) % 4
        }
        add cell2 (distance+1000)

        let cell3 = { 
            Kind = newcell.Kind       
            Row = newcell.Row;
            Col = newcell.Col;
            Orientation = (newcell.Orientation + 3) % 4
        }
        add cell3 (distance+1000)
    
    (minValueAtCell dimensions endnode) - 1000

let execute() =
    let path = "day16/day16_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, startNode, endNode) = parseContent content
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))
    let arrayofdimensions = Array.init maxrows (fun _ -> 
        Array.init maxcols (fun _ -> 
            Array.create 4 System.Int32.MaxValue
        )
    )
    walk (startNode, endNode) arrayofdimensions map