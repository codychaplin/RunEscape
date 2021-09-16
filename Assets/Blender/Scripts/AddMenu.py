import bpy

class SimpleOperator(bpy.types.Operator):
        bl_idname = "object.simple_operator"
        bl_label = "Button text"

        @classmethod
        def execute(self, context):
            print("Hello world!")
            return {'FINISHED'}

class DissolvePanel(bpy.types.Panel):
    bl_label = "Scripts Panel"
    bl_idname = "ScriptsPanel"
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = 'Scripts'

    def draw(self, context):
        layout = self.layout
        row = layout.row()
        row.label(text="Scripts", icon= 'FILE_SCRIPT')
        row = layout.row()
        #bpy.data.texts["AddModifier"].as_module()
        row.operator("object.simple_operator")

def register():
    bpy.utils.register_class(DissolvePanel)

def unregister():
    bpy.utils.unregister_class(DissolvePanel)

if __name__ == "__main__":
    register()
    #unregister()