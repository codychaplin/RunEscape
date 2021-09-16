import bpy

mat = bpy.data.materials["Pavement"].name
for obj in bpy.context.selected_objects:
    polys = obj.data.polygons
    if polys.data.materials.find(mat) == -1:
        polys.data.materials.append(bpy.data.materials[mat])
    for poly in polys:
        if poly.select:
            print(bpy.data.materials.find(mat))
            poly.material_index = bpy.data.materials.find(mat)