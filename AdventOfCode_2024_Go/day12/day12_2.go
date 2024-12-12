package day12

import (
	"fmt"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type Direction int

const (
	Up    Direction = 1
	Down  Direction = 2
	Right Direction = 3
	Left  Direction = 4
)

func nextPosition(movDir Direction, position Coord) Coord {
	switch movDir {
	case Up:
		return Coord{position.Row - 1, position.Col}
	case Down:
		return Coord{position.Row + 1, position.Col}
	case Right:
		return Coord{position.Row, position.Col + 1}
	case Left:
		return Coord{position.Row, position.Col - 1}
	}
	return Coord{}
}

func growDimension(region, visitedSoFar map[Coord]struct{}, movDir Direction, point Coord) map[Coord]struct{} {
	var nextPoint Coord
	if movDir == Up || movDir == Down {
		nextPoint = nextPosition(Right, point)
	} else {
		nextPoint = nextPosition(Down, point)
	}

	if _, ok := region[nextPoint]; ok {
		secondStep := nextPosition(movDir, nextPoint)
		if _, ok := region[secondStep]; !ok {
			visitedSoFar[point] = struct{}{}
			return growDimension(region, visitedSoFar, movDir, nextPoint)
		}
	}
	visitedSoFar[point] = struct{}{}
	return visitedSoFar
}

func consumeRegion(name string, movDir Direction, region, visited map[Coord]struct{}, regionPoints []Coord, numOfSides int) int {
	if len(regionPoints) == 0 {
		return numOfSides
	}

	currentPoint := regionPoints[0]
	restPoints := regionPoints[1:]
	if _, ok := visited[currentPoint]; ok {
		return consumeRegion(name, movDir, region, visited, restPoints, numOfSides)
	}

	nextPoint := nextPosition(movDir, currentPoint)
	if _, ok := region[nextPoint]; ok {
		visited[currentPoint] = struct{}{}
		return consumeRegion(name, movDir, region, visited, restPoints, numOfSides)
	}

	newVisited := growDimension(region, visited, movDir, currentPoint)
	numOfSides++
	fmt.Printf("visited: %v\n", newVisited)
	fmt.Printf("New side for %v at %v\n", name, currentPoint)
	return consumeRegion(name, movDir, region, newVisited, restPoints, numOfSides)
}

func exploreRegion(region Region) (sides int) {
	for _, dir := range []Direction{Up, Down, Right, Left} {
		regionMap := make(map[Coord]struct{})
		for _, p := range region.Points {
			regionMap[p] = struct{}{}

		}
		sides += consumeRegion(region.Name, dir, regionMap, make(map[Coord]struct{}), region.Points, 0)
	}
	return
}

func Executepart2() int {
	var result int = 0

	// var fileName string = "./day12/day12.txt"

	var fileName string = "./day12/test_input_12.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		// result = len(fileContent)
		garden := make([][]string, 0)
		for _, line := range fileContent {
			garden = append(garden, strings.Split(line, ""))
		}
		maxRows, maxCols := len(garden), len(garden[0])
		regions := buildRegions(garden, maxRows, maxCols)
		for _, reg := range regions {
			sides := exploreRegion(reg)
			result += reg.Size * sides
			fmt.Printf("Region %v - Size %v - Sides %v\n", reg.Name, reg.Size, sides)
		}
	}

	return result
}
