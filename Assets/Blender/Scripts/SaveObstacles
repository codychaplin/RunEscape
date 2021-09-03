import bpy

path = bpy.path.abspath("//")
text = open(path + "..\WorldData\Obstacles.txt", "w")
col = bpy.data.collections['Obstacles']
n = '\n'

def addCoords(x, y, z):
    text.write(str(int(x)) + ",")
    text.write(str(int(y)) + ",")
    text.write(str(int(z)) + n)

for obj in col.objects:
    #location data
    xLoc = int(obj.location[0])
    yLoc = int(obj.location[1])
    zLoc = int(obj.location[2])
    #size data
    xDim = int(obj.dimensions[0])
    yDim = int(obj.dimensions[1])
    zDim = int(obj.dimensions[2])

    if xDim > 1 and yDim > 1:
        for x in range(xLoc, xLoc + xDim):
            for y in range (yLoc, yLoc + yDim):
                addCoords(x, y, zLoc)
    else:
        addCoords(xLoc, yLoc, zLoc) # adds origin vertex

text.close()