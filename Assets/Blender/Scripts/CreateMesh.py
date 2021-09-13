import bpy

verts = []
edges = []
faces = []

verts.append([ 0.0, 0.0, 0.0]) # x, y, z
verts.append([ 0.0, 1.0, 0.0])
verts.append([ 1.0, 0.0, 0.0])
verts.append([ 1.0, 1.0, 0.0])

faces.append([0, 1, 3, 2])

name = "TestObject"
mesh = bpy.data.meshes.new(name)
obj = bpy.data.objects.new(name, mesh)
col = bpy.data.collections.new("TestCol")
bpy.context.scene.collection.children.link(col)
col.objects.link(obj)
bpy.context.view_layer.objects.active = obj
mesh.from_pydata(verts, edges, faces)