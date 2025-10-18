import sys
from pathlib import Path
from ScriptCollection.ScriptCollectionCore import ScriptCollectionCore
from ScriptCollection.TFCPS.TFCPS_Tools_General import TFCPS_Tools_General


def run_example():
    file = str(Path(__file__).absolute())
    sc=ScriptCollectionCore()
    t = TFCPS_Tools_General(sc)
    t.run_dockerfile_example(file, 0, True, True, sys.argv)
    # HINT now open https://<domain>:443/API/Other/Resources/APISpecification/index.html in your browser.


if __name__ == "__main__":
    run_example()
