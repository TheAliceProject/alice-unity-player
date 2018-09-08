@echo off

for /d %%d in (src/*) do (
	buildA3P %%d
)

popd