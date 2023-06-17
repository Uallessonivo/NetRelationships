namespace NetRelationships.Dtos
{
    public record struct CreateCharacterDto(
        string Name, 
        CreateBackpackDto BackpackDto, 
        List<CreateWeaponDto> WeaponDtos, 
        List<CreateFactionDto> FactionDtos
     );
}
