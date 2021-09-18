import bpy

name = "Shrinkwrap"
#WestBeach
#SouthBeach
targetName = "path"
vGroup = True
for obj in bpy.context.selected_objects:
    if name in obj.modifiers:
        mod = obj.modifiers[name]
    else:
        mod = obj.modifiers.new(name=name, type=str.upper(name))
        
    mod.wrap_method = 'PROJECT'
    mod.wrap_mode = 'ON_SURFACE'
    mod.use_project_z = True
    mod.target = bpy.data.objects[targetName]
    if vGroup:
        mod.vertex_group = obj.vertex_groups['grass'].name