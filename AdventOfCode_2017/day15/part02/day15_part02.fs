module day15_part02

open AdventOfCode_2017.Modules

type Generator = {
    Factor: int64
    CurrentValue: int64
}

let parseContent(lines: string array) =
    let (startA, startB) = ((int64)(lines[0].Split(" ")[4]), (int64)(lines[1].Split(" ")[4]))
    (startA, startB)

let nextValue(generator: Generator) =
    { generator with CurrentValue = (generator.CurrentValue * generator.Factor) % 2147483647L }

let decimalToBinary (number: int64) =
    let binary =
        if number = 0L then "0"
        else
            let rec convert n acc =
                if n = 0L then acc
                else convert (n / 2L) (string (n % 2L) + acc)
            convert number ""

    if binary.Length < 16 then 
        binary.PadLeft(16, '0')
    else
        binary.Substring(binary.Length - 16)
    
let judgeValues(counter: int) ((generatorA, generatorB) : Generator * Generator) =
    let mutable total = 0
    let genACollection = ResizeArray<int64>()
    let genBCollection = ResizeArray<int64>()

    let rec generate c gA gB =
        if c % 100000 = 0 then
            printfn "counter: %d. A size: %d - B size: %d" c genACollection.Count genBCollection.Count
        match (genACollection.Count = counter, genBCollection.Count = counter) with
        | (true, true) -> (generatorA, generatorB)
        | _ ->
            let newA = nextValue gA
            let newB = nextValue gB
            if newA.CurrentValue % 4L = 0L && genACollection.Count < counter then
                genACollection.Add(newA.CurrentValue)
            if newB.CurrentValue % 8L = 0L && genBCollection.Count < counter then
                genBCollection.Add(newB.CurrentValue)
            //if (decimalToBinary newA.CurrentValue) = (decimalToBinary newB.CurrentValue) then
            //    total <- total + 1
            generate (c + 1) newA newB

    let _ = generate 0 generatorA generatorB
    for c in 0..counter-1 do
        if (decimalToBinary genACollection[c]) = (decimalToBinary genBCollection[c]) then
            total <- total + 1
    total

let execute() =
    let path = "day15/day15_input.txt"
    //let path = "day15/test_input_15.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (startA, startB) = parseContent content

    let comparisons = 5000000
    //let comparisons = 5

    let(factorA, factorB) = (16807L, 48271L)
    let generatorA = { Factor = factorA; CurrentValue = startA }
    let generatorB = { Factor = factorB; CurrentValue = startB }
    judgeValues comparisons (generatorA, generatorB)