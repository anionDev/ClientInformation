from pathlib import Path
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.ScriptCollectionCore import ScriptCollectionCore
from ScriptCollection.TasksForCommonProjectStructure import TasksForCommonProjectStructure


def update_iplocation_submodule(repository_folder):
    submodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    sc: ScriptCollectionCore = ScriptCollectionCore()
    sc.git_fetch(submodule_folder, "origin")
    sc.git_checkout(submodule_folder, "main")
    sc.git_pull(submodule_folder, "origin", "main", "main")


def prepare_build_codeunits():
    t = TasksForCommonProjectStructure()
    current_file = str(Path(__file__).absolute())
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    t.ensure_certificate_authority_for_development_purposes_is_generated(current_file)
    update_iplocation_submodule(repository_folder)


if __name__ == "__main__":
    prepare_build_codeunits()
