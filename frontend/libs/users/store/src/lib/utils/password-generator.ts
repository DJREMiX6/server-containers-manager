const PasswordUppercaseChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
const PasswordLowercaseChars = 'abcdefghijklmnopqrstuvwxyz';
const PasswordDigitChars = '0123456789';
const PasswordSpecialChars = '!@#$%^&*()_-+=[]{};;:\'",./<>?\\|`~';
const AllPasswordChars =
  PasswordUppercaseChars +
  PasswordLowercaseChars +
  PasswordDigitChars +
  PasswordSpecialChars;

export function generatePassword(): string {
  const randomIndex = (max: number): number =>
    crypto.getRandomValues(new Uint32Array(1))[0] % max;

  const required = [
    PasswordUppercaseChars[randomIndex(PasswordUppercaseChars.length)],
    PasswordLowercaseChars[randomIndex(PasswordLowercaseChars.length)],
    PasswordDigitChars[randomIndex(PasswordDigitChars.length)],
    PasswordSpecialChars[randomIndex(PasswordSpecialChars.length)],
  ];

  const remaining = Array.from(
    { length: 8 },
    () => AllPasswordChars[randomIndex(AllPasswordChars.length)],
  );

  const chars = [...required, ...remaining];
  for (let i = chars.length - 1; i > 0; i--) {
    const j = randomIndex(i + 1);
    [chars[i], chars[j]] = [chars[j], chars[i]];
  }

  return chars.join('');
}
