import os
import glob
import re

base_dir = r"d:\workspace\LenteraNusantaraTest\Assets\JyotisSugata"
cs_files = glob.glob(os.path.join(base_dir, "**", "*.cs"), recursive=True)

usings_to_add = """using JyotisSugata.Core.Events;
using JyotisSugata.Core.StateMachine;
using JyotisSugata.Core.Input;
using JyotisSugata.Exploration.Player;
using JyotisSugata.Exploration.Interaction;
using JyotisSugata.Puzzles.Shared;
using JyotisSugata.Puzzles.MemoryMatch;
using JyotisSugata.Puzzles.NumpadPasscode;
using JyotisSugata.UI.HUD;
using JyotisSugata.UI.Transitions;
"""

for filepath in cs_files:
    rel_path = os.path.relpath(filepath, base_dir)
    parts = rel_path.split(os.sep)[:-1] # exclude filename
    
    # Clean up parts
    clean_parts = []
    for p in parts:
        if p.startswith("_"):
            clean_parts.append(p[1:])
        else:
            clean_parts.append(p)
            
    target_namespace = "JyotisSugata"
    if clean_parts:
        target_namespace += "." + ".".join(clean_parts)
        
    with open(filepath, 'r', encoding='utf-8-sig') as f:
        content = f.read()
        
    # Add usings at the top if not present
    if "using JyotisSugata.Core.Events;" not in content:
        content = usings_to_add + "\n" + content
        
    # Replace namespace declaration exactly
    content = re.sub(r"namespace\s+JyotisSugata\s*\{", f"namespace {target_namespace}\n{{", content)
    
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)
        
print(f"Updated {len(cs_files)} files with new namespaces and using directives.")
