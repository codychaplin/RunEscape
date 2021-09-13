bl_info = {"name" : "Scripts",   "category": "3D View",   "author": "Cody"}
import bpy
class Scripts(bpy.types.Menu):
    bl_label = "Scripts"
    bl_idname = "Scripts"
    def draw(self, context):
        layout = self.layout
        
        layout.operator("mesh.primitive_cube_add")
        layout.operator("mesh.primitive_circle_add")
        layout.operator("object.duplicate_move")
        layout.operator("object.delete")
        layout.operator("object.armature_add")
        layout.operator("object.posemode_toggle")
        
def register(): bpy.utils.register_class(Scripts)
def unregister(): bpy.utils.unregister_class(Scripts)
if __name__ == "__main__": register()