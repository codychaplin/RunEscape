import bpy

path = bpy.path.abspath("//")
text = open(path + "..\WorldData\Vertices.txt", "w")
obj = bpy.data.objects[0]
verts = obj.data.vertices
faces = obj.data.polygons

i = 0
isLast = False

for v in verts[:-17]:
    if isLast == False:
        coords = obj.matrix_world @ v.co
        #coords = v.co
        
        for c in coords[:-1]:
            text.write(str(int(c)) + ",")
            
        if i == 15:
            text.write(str(int(coords[-1])) + '\n')
            i = 0
            isLast = True
        else:
            text.write(str(int(coords[-1])) + "|")
            i += 1
    else:
        isLast = False
    
text.close()