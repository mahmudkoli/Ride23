build:
	@dotnet build /ll

tye:
	@tye run

docker-up:
	@docker-compose -f docker-compose.infrastructure.yml up -d

docker-down:
	@docker-compose -f docker-compose.infrastructure.yml down

dotnet-publish:
	@cd ./src/gateways/Gateway && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/customer/Ride23.Customer.API && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./src/services/identity/Ride23.Identity.API && dotnet publish --os linux --arch x64 -c Release --self-contained
