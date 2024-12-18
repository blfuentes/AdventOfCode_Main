package utilities

import (
	"container/heap"
)

// Item represents a single element in the priority queue.
type Item[T any] struct {
	value    T   // The value of the item (generic type).
	priority int // The priority of the item.
	index    int // The index of the item in the heap (required for updating).
}

// PriorityQueue implements the heap.Interface and holds Items.
type PriorityQueue[T any] []*Item[T]

// Len is the number of elements in the collection.
func (pq PriorityQueue[T]) Len() int { return len(pq) }

// Less compares the priority of two elements (higher priority comes first).
func (pq PriorityQueue[T]) Less(i, j int) bool {
	return pq[i].priority > pq[j].priority
}

// Swap swaps two elements in the collection.
func (pq PriorityQueue[T]) Swap(i, j int) {
	pq[i], pq[j] = pq[j], pq[i]
	pq[i].index = i
	pq[j].index = j
}

// Push adds an element to the collection.
func (pq *PriorityQueue[T]) Push(x any) {
	n := len(*pq)
	item := x.(*Item[T])
	item.index = n
	*pq = append(*pq, item)
}

// Pop removes and returns the element with the highest priority.
func (pq *PriorityQueue[T]) Pop() any {
	old := *pq
	n := len(old)
	item := old[n-1]
	item.index = -1 // For safety
	*pq = old[0 : n-1]
	return item
}

// Update modifies the priority and value of an Item in the queue.
func (pq *PriorityQueue[T]) Update(item *Item[T], value T, priority int) {
	item.value = value
	item.priority = priority
	heap.Fix(pq, item.index)
}
