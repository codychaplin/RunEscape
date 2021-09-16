import bpy

size = 16*7+1 # grid is 16x16 but n+1 vertices is needed

def vert(x, y):
    return (x, y, 0)

def face(x, y):
    return (x * size + y,
        (x + 1) * size + y,
        (x + 1) * size + 1 + y,
        x * size + 1 + y)

#creates lists of vertices and faces
verts = [vert(x, y) for x in range(size) for y in range(size)]
faces = [face(x, y) for x in range(size - 1) for y in range(size - 1)]

# creates mesh
name = "Chunk"
mesh = bpy.data.meshes.new(name)
mesh.from_pydata(verts, [], faces)

# create object and link to collection
obj = bpy.data.objects.new(name, mesh)
col = bpy.data.collections['World']
col.objects.link(obj)

# adds triangulate modifier
obj.modifiers.new(name = "Triangulate", type = 'TRIANGULATE')