package day04

import (
	"fmt"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart2() int {
	var fileName string = "./day04/day04.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		fmt.Print(fileContent)
	}

	return result
}
