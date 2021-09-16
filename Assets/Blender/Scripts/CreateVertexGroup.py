import bpy

name = "edge"
for obj in bpy.context.selected_objects:
    verts = obj.data.vertices
    if obj.vertex_groups.find(name) != -1:
        vGroup = obj.vertex_groups[name]
    else:
        vGroup = obj.vertex_groups.new(name = name)
    
    selectedVerts = []
    for vert in verts:
        if vert.select:
            selectedVerts.append(vert.index)
    vGroup.add(selectedVerts, 1, 'REPLACE')