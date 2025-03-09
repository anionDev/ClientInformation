import sys
from pathlib import Path
from ScriptCollection.TasksForCommonProjectStructure import TasksForCommonProjectStructure


def build():
    t = TasksForCommonProjectStructure()
    build_script_file = str(Path(__file__).absolute())
    t.standardized_tasks_build_for_docker_project(build_script_file, "QualityCheck", 1, sys.argv)
    t.merge_sbom_file_from_dependent_codeunit_into_this(build_script_file, "ClientInformation")


if __name__ == "__main__":
    build()
