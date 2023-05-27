build: ## Build .NET project inside src
	@echo Building source code...
	cd src && dotnet build --nologo && cd ..

clean: ## Clean all obj and .vs folders
	@echo Cleaning Visual Studio Craps...
	rm -Rf ./src/obj
	rm -Rf .vs

help: ## Show usage and recipes (default)
	@printf "Usage: make <recipe> [...]\n"
	@printf "\nHere is the list of recipes for you:\n"
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  \033[36m%-20s\033[0m %s\n", $$1, $$2}' Makefile

.DEFAULT_GOAL := help
.PHONY: help clean build
