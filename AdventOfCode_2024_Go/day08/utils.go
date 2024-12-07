package day08

func concat(a, b int64) int64 {
	multiplier := int64(1)
	for temp := b; temp > 0; temp /= 10 {
		multiplier *= 10
	}
	return a*multiplier + b
}
