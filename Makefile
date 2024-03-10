build:
	@dotnet build /ll

tye:
	@tye run

tye-watch:
	@tye run --watch

docker-build:
	@docker-compose -f docker-compose.yml build

docker-pull:
	@docker-compose -f docker-compose.infrastructure.yml pull

docker-up:
	@docker-compose -f docker-compose.infrastructure.yml -f docker-compose.yml -f docker-compose.override.yml up -d

docker-infra-up:
	@docker-compose -f docker-compose.infrastructure.yml up -d

docker-down:
	@docker-compose -f docker-compose.infrastructure.yml -f docker-compose.yml -f docker-compose.override.yml down

docker-app-down:
	@docker-compose -f docker-compose.yml down

dotnet-publish:
	@cd ./src/gateways/Gateway && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/customer/Ride23.Customer.API && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/identity/Ride23.Identity.API && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/driver/Ride23.Driver.API && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/location/Ride23.Location.API && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/order/Ride23.Order.API && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/inventory/Ride23.Inventory.API && dotnet publish --os linux --arch x64 -c Release --self-contained
