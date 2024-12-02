package day03

import (
	"fmt"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var fileName string = "./day03/day03.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		fmt.Print(fileContent)
	}

	return result
}
