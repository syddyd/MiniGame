Sydney Dacks, COMP 521, 2024-09-16

My lake-crossing algorithm was inspired by Prim's algorithm. I realized that the question was asking us to make a unicursal random maze. 

I made an 8x10 grid next to the tower which is meant to be knocked over, made of rows and columns.  
Algorithm: 
while the other side has not been reached 
	Choose a random stone s from the row of the grid closest to you
	Add s to the path
	Explore each of s's neighbors if they haven't been explored already 
	add them to the frontier, and set s as their predecessors
	randomly select a new s from the frontier

out of the while loop: backtrack through the predecessors to find a path which crosses the lake

This algo is much like Prim's except for the fact that I always start at the shore closest to the tower instead of a random node, and end as soon as I touch the other shore, not necessarily  completing the maze. It has some bias but is quite fast. 