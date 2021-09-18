import bpy

name = "Triangulate"
for obj in bpy.context.selected_objects:
    if name in obj.modifiers:
        mod = obj.modifiers[name]
        obj.modifiers.remove(mod)