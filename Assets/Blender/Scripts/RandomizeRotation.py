import bpy
from math import radians
from random import choice

rots = [0, 90, 180, 270]

for obj in bpy.context.selected_objects:
    obj.rotation_euler.z = radians(choice(rots))