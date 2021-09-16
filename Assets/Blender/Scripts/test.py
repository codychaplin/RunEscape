import bpy

class DissolvePanel(bpy.types.Panel):
    bl_label = "Dissolve Panel"
    bl_idname = "OBJECT_PT_hello"
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'TOOLS'

    def draw(self, context):
        layout = self.layout
        col = layout.column(align=True)
        row = col.row(align=True)
        row.operator("mesh.dissolve_edges", text = "Dissolve Edges")
        row.operator("mesh.dissolve_verts", text = "Dissolve Verts")
        # invoke custom operator
        # row.operator("object.simple_operator" , text = "All")

def register():
    bpy.utils.register_class(DissolvePanel)

def unregister():
    bpy.utils.unregister_class(DissolvePanel)

if __name__ == "__main__":
    register()