import bpy
import math
import numpy as np

path = bpy.path.abspath("//") # project path
text = open(path + "..\WorldData\Vertices.txt", "w") # file path
col = bpy.data.collections['World'] # collection named "World"
chunks = col.objects # list of chunks (children of World)
dim = int(math.sqrt(len(chunks))) # gets the length of 1 dimension of list
chunkArray = np.array(chunks).reshape(dim, dim) # converts list to 2D array
n = '\n'

for index in range(0, 2): # column index
    for row in range(0, 16): # row index
        for chunk in chunkArray[index]: # chunk
            verts = np.array(chunk.data.vertices).reshape(17,17) # 2D vertex array
            for vertColumn in range(0, 16): # column index
                coords = chunk.matrix_world @ verts[row][vertColumn].co # vertex location
                for c in coords[:-1]: # x, y, z coords
                    text.write(str(int(c)) + ",")
                text.write(str(int(coords[-1])) + "|")
        text.write(n)
    
text.close()