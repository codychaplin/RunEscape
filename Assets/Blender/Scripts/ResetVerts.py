import bpy

#obj = bpy.data.objects[0]
#verts = obj.data.vertices
for obj in bpy.context.selected_objects:
    for vert in obj.data.vertices:
        vert.co.z = 0