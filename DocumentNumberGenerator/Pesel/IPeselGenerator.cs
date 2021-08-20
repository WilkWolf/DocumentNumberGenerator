using System.Collections.Generic;

namespace DocumentNumberGenerator.Pesel
{
    interface IPeselGenerator
    {
        List<string> Generate(PeselSettingsModel settings);
    }
}
