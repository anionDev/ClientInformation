from pathlib import Path
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.ScriptCollectionCore import ScriptCollectionCore
from ScriptCollection.TFCPS.TFCPS_Tools_General import TFCPS_Tools_General


def prepare_build_codeunits():
    sc=ScriptCollectionCore()
    t = TFCPS_Tools_General(sc)
    current_file = str(Path(__file__).absolute())
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    t.ensure_certificate_authority_for_development_purposes_is_generated(repository_folder)
    t.generate_certificate_for_development_purposes_for_product(repository_folder)
    t.generate_tasksfile_from_workspace_file(repository_folder)
    t.generate_codeunits_overview_diagram(repository_folder)
    t.generate_svg_files_from_plantuml_files_for_repository(repository_folder,False)


if __name__ == "__main__":
    prepare_build_codeunits()
