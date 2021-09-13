import bpy

name = "Displace"
for obj in bpy.context.selected_objects:
    if name in obj.modifiers:
        mod = obj.modifiers[name]
    else:
        mod = obj.modifiers.new(name=name, type="DISPLACE")
        
    mod.texture = bpy.data.textures["Noise"]
    mod.texture_coords = 'GLOBAL'
    mod.strength = 0.25