import bpy

name = "Displace"
vGroup = "edge"
for obj in bpy.context.selected_objects:
    if name in obj.modifiers:
        mod = obj.modifiers[name]
    else:
        mod = obj.modifiers.new(name=name, type=str.upper(name))
        
    mod.texture = bpy.data.textures["Noise"]
    mod.texture_coords = 'GLOBAL'
    mod.strength = 0.25
    mod.vertex_group = obj.vertex_groups[vGroup].name