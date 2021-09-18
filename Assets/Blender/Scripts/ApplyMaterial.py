import bpy

default = False;
mat = bpy.data.materials["White"]
for obj in bpy.context.selected_objects:
    if default:
        obj.data.materials.append(mat)
    else:
        polys = obj.data.polygons
        if polys.data.materials.find(mat.name) == -1:
            polys.data.materials.append(bpy.data.materials[mat.name])
        for poly in polys:
            if poly.select:
                poly.material_index = bpy.data.materials.find(mat.name)